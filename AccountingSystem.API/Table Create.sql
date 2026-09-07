CREATE TABLE users
(
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL,
    password_hash VARCHAR(500) NOT NULL,
    full_name VARCHAR(200) NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT uq_users_username UNIQUE (username)
);

CREATE TABLE roles
(
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,

    CONSTRAINT uq_roles_name UNIQUE (name)
);

CREATE TABLE user_roles
(
    user_id INT NOT NULL,
    role_id INT NOT NULL,

    CONSTRAINT pk_user_roles PRIMARY KEY (user_id, role_id),

    CONSTRAINT fk_user_roles_users
        FOREIGN KEY (user_id)
        REFERENCES users (id),

    CONSTRAINT fk_user_roles_roles
        FOREIGN KEY (role_id)
        REFERENCES roles (id)
);

-- default user
INSERT INTO roles (name)
VALUES ('Admin');
INSERT INTO roles (name)
VALUES
    ('Manager'),
    ('Accountant'),
    ('Sales'),
    ('Purchase'),
    ('User');


INSERT INTO users
(
    username,
    password_hash,
    full_name,
    is_active
)
VALUES
(
    'admin',
    '$2a$11$JENtwlgz1sXI0tCRWfnRqOgGOq3zINlSJEQyFAlV5yVctJfCEUvFO',
    'System Administrator',
    TRUE
);


INSERT INTO user_roles (user_id, role_id)
SELECT
    u.id,
    r.id
FROM users u
CROSS JOIN roles r
WHERE u.username = 'admin'
  AND r.name = 'Admin';
